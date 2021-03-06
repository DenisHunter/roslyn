﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Editor.Shared.Options;
using Microsoft.CodeAnalysis.Editor.Shared.Tagging;
using Microsoft.CodeAnalysis.Editor.Shared.Utilities;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Shared.Options;
using Microsoft.CodeAnalysis.Shared.TestHooks;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.CodeAnalysis.Editor.Implementation.Diagnostics
{
    [Export(typeof(ITaggerProvider))]
    [ContentType(ContentTypeNames.CSharpContentType)]
    [ContentType(ContentTypeNames.VisualBasicContentType)]
    [TagType(typeof(ClassificationTag))]
    internal partial class DiagnosticsClassificationTaggerProvider : AbstractDiagnosticsTaggerProvider<ClassificationTag>
    {
        private readonly ClassificationTypeMap _typeMap;
        private static readonly IEnumerable<Option<bool>> s_tagSourceOptions = new[] { EditorComponentOnOffOptions.Tagger, InternalFeatureOnOffOptions.Classification, ServiceComponentOnOffOptions.DiagnosticProvider };

        [ImportingConstructor]
        public DiagnosticsClassificationTaggerProvider(
            IDiagnosticService service,
            IForegroundNotificationService notificationService,
            ClassificationTypeMap typeMap,
            [ImportMany] IEnumerable<Lazy<IAsynchronousOperationListener, FeatureMetadata>> listeners)
            : base(service, notificationService, new AggregateAsynchronousOperationListener(listeners, FeatureAttribute.Classification))
        {
            _typeMap = typeMap;
        }

        protected override IEnumerable<Option<bool>> TagSourceOptions
        {
            get
            {
                return s_tagSourceOptions;
            }
        }

        protected override AbstractAggregatedDiagnosticsTagSource<ClassificationTag> CreateTagSourceCore(ITextView textViewOpt, ITextBuffer subjectBuffer)
        {
            if (this.DiagnosticService == null)
            {
                return null;
            }

            return new TagSource(subjectBuffer, this.NotificationService, this.DiagnosticService, _typeMap, this.AsyncListener);
        }
    }
}
