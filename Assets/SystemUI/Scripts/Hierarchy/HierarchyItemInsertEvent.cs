using System;

namespace Previz.Hierarchy
{
    /// <summary>
    /// ヒエラルキー上でアイテムの間にアイテムをdrag & dropした際に使用するデータ構造
    /// </summary>
    public record HierarchyItemInsertEvent
    {
        /// <summary>
        /// 挿入する位置の上にあるアイテムID
        /// </summary>
        public Guid? AboveItemId;
        
        /// <summary>
        /// 移動させたアイテムID
        /// </summary>
        public Guid MoveItemId;
    }
}