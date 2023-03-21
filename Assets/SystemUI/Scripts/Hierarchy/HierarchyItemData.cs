using System;
using System.Collections.Generic;

namespace Previz.Hierarchy
{
    public record HierarchyItemData
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public List<HierarchyItemData> ChildrenItems { get; init; }
    }
}