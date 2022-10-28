using UnityEngine.UI;

namespace inc.stu.SystemUI
{
    public class FloatMinMaxSlider : MinMaxSlider<float>
    {

        protected override void SetMinMaxToSliderComponent(Slider sliderComponent)
        {
            sliderComponent.minValue = _minValue;
            sliderComponent.maxValue = _maxValue;
            sliderComponent.wholeNumbers = false;
        }

        protected override float CastSliderFloatToGenericValue(float value)
        {
            return value;
        }

        protected override float CastGenericValueForSliderFloat(float value)
        {
            return value;
        }

        protected override float Parse(string value) => float.TryParse(value, out var result) ? result : 0;

    }

}

