using UnityEngine.UI;

namespace inc.stu.SystemUI
{
    public class IntMinMaxSlider : MinMaxSlider<int>
    {

        protected override void SetMinMaxToSliderComponent(Slider sliderComponent)
        {
            sliderComponent.minValue = _minValue;
            sliderComponent.maxValue = _maxValue;
            sliderComponent.wholeNumbers = true;
        }

        protected override int CastSliderFloatToGenericValue(float value)
        {
            return (int)value;
        }

        protected override float CastGenericValueForSliderFloat(int value)
        {
            return value;
        }

        protected override int Parse(string value) => int.TryParse(value, out var result) ? result : 0;

    }

}

