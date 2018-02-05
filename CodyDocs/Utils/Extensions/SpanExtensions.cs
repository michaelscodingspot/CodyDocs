using CodyDocs.Services;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodyDocs.Utils
{
    internal static class SpanExtensions
    {
        private const PositionAffinity positionAffinity = PositionAffinity.Successor;

        public static Span GetSpan(this IMappingSpan mappingTagSpan)
        {
            var buffer = mappingTagSpan.AnchorBuffer;
            var startSnapshotPoint = mappingTagSpan.Start.GetPoint(buffer, positionAffinity).Value;
            var endSnapshotPoint = mappingTagSpan.End.GetPoint(buffer, positionAffinity).Value;
            var length = endSnapshotPoint.Position - startSnapshotPoint.Position;
            return new Span(startSnapshotPoint.Position, length);
        }

        public static SnapshotSpan GetSnapshotSpan(this IMappingSpan mappingTagSpan)
        {
            var buffer = mappingTagSpan.AnchorBuffer;
            var span = GetSpan(mappingTagSpan);
            var snapshot = mappingTagSpan.Start.GetPoint(buffer, positionAffinity).Value.Snapshot;
            return new SnapshotSpan(snapshot, span);
        }



    }
}
