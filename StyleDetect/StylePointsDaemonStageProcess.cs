using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Naming;
using JetBrains.ReSharper.Psi.Naming.Impl;
using JetBrains.ReSharper.Psi.Naming.Interfaces;
using JetBrains.ReSharper.Psi.Naming.Settings;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Naming.Impl;
using NLog;

namespace Codevolve.StyleDetect
{
    /// <summary>
    /// Daemon stage process to gather and analyze "style points" (code examples that show particular code style).
    /// </summary>
    public class StylePointsDaemonStageProcess : IDaemonStageProcess
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="StylePointsDaemonStageProcess"/> class.
        /// </summary>
        /// <param name="process">The associated daemon process.</param>
        public StylePointsDaemonStageProcess(IDaemonProcess process)
        {
            DaemonProcess = process;
        }

        #region IDaemonStageProcess Members

        /// <summary>
        /// Executes the process.
        /// The process should check for <see cref="P:JetBrains.ReSharper.Daemon.IDaemonProcess.InterruptFlag"/> periodically (with intervals less than 100 ms)
        /// and throw <see cref="T:JetBrains.Application.Progress.ProcessCancelledException"/> if it is true.
        /// Failing to do so may cause the program to prevent user from typing while analysing the code.
        /// Stage results should be passed to <param name="commiter"/>. If DaemonStageResult is <c>null</c>, it means that no highlightings available
        /// </summary>
        public void Execute(Action<DaemonStageResult> commiter)
        {
            if (DaemonProcess.InterruptFlag)
            {
                return;
            }

            PsiManager manager = PsiManager.GetInstance(DaemonProcess.Solution);
            var sourceFile = manager.GetPsiFile(DaemonProcess.SourceFile, CSharpLanguage.Instance) as ICSharpFile;
            if (sourceFile != null)
            {
                // process declarations

                var processor = new RecursiveElementProcessor<IDeclaration>(
                    declaration =>
                        {
                            IDeclaredElement element = declaration.DeclaredElement;

                            logger.Trace("Processing a declaration {0}.", declaration.DeclaredName);

                            PsiLanguageType languageType = element.PresentationLanguage;
                            NamingManager namingManager = element.GetPsiServices().Naming;
                            NameParser nameParser = namingManager.Parsing;
                            var source = DaemonProcess.SourceFile;

                            INamingPolicyProvider policyProvider = namingManager.Policy.GetPolicyProvider(languageType, source);
                            NamingPolicy namingPolicy = GetNamingPolicy(element, source);
                            Name name = nameParser.Parse(declaration.DeclaredName, namingPolicy.NamingRule, languageType, source);

                            logger.Trace("Name: {0}, HasErrors: {1}", name, name.HasErrors);
                            logger.Trace("Naming policy: {0}", namingPolicy);
                        });
                sourceFile.ProcessDescendants(processor);
            }
        }

        /// <summary>
        /// Whole daemon process
        /// </summary>
        public IDaemonProcess DaemonProcess { get; private set; }

        #endregion

        /// <summary>
        /// Returns naming policy for specified declared element.
        /// </summary>
        /// <param name="element">The declared element to look policy for.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <returns>
        /// Corresponding naming policy.
        /// </returns>
        private NamingPolicy GetNamingPolicy(IDeclaredElement element, IPsiSourceFile sourceFile)
        {
            PsiLanguageType languageType = element.PresentationLanguage;
            NamingManager namingManager = element.GetPsiServices().Naming;
            INamingPolicyProvider policyProvider = namingManager.Policy.GetPolicyProvider(languageType, sourceFile);
            
            NamingPolicy namingPolicy = policyProvider.GetPolicy(element);

            if (namingPolicy.ExtraRules.Any())
            {
                logger.Trace("Extra rules are found!");
            }

            return namingPolicy;
        }
    }
}