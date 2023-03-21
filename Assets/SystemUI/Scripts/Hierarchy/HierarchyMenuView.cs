using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Previz.Hierarchy
{
    public class HierarchyMenuView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rootNode;
        [SerializeField] private BoxCollider2D _collider;

        private readonly Subject<Unit> _onClickSubject = new();
        private bool _isChildMenuDisplaying = false;
        private GameObject _childMenuGameObject;

        public IObservable<Unit> OnClick => _onClickSubject;
        
        public void SetInstantiatedChildMenu(HierarchyMenuView childMenu)
        {
            _childMenuGameObject = childMenu.gameObject;
            _isChildMenuDisplaying = true;
        }

        public void CheckInstantiatedChildMenu()
        {
            if (_isChildMenuDisplaying)
            {
                Destroy(_childMenuGameObject);
                _isChildMenuDisplaying = false;
            }
        }

        public IEnumerator ApplyColliderSize()
        {
            // ContentSizeFitterのサイズが適応を待つために1フレーム待機
            yield return null;
            _collider.size = new Vector2(_rootNode.rect.width, _rootNode.rect.height);
            _collider.offset = new Vector2(_rootNode.rect.width / 2f, -_rootNode.rect.height / 2f);
        }
    }
}