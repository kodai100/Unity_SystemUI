using UnityEngine;


namespace inc.stu.SystemUI
{

    public class Vector3Parameter : Parameter<Vector3>
    {

        [SerializeField] private Vector3Field _field;

        public override Field<Vector3> Field => _field;

    }

}
