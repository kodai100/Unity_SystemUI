using System;

namespace Previz.Hierarchy
{
    public record HierarchyItemDropInEvent
    {
        public Guid? ParentItemId;
        public Guid MoveItemId;
        public int Index;
    }
}