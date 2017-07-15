// Copyright 2017 Daniel Plemmons

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Denizen.Utils;

namespace ThreeSpace
{
  [ExecuteInEditMode]
  public class ScalarView : MonoBehaviour
  {
    [SerializeField]
    private Transform _meter;

    [SerializeField]
    private Transform _positiveBackground;

    [SerializeField]
    private Transform _negativeBackground;

    [SerializeField]
    private TextMesh _meterText;

    [SerializeField]
    private float _max;

    [SerializeField]
    private float _min;

    [SerializeField]
    private float _height;

    [SerializeField]
    private float _scalarValue;

    private const float BACKGROUND_ASSET_HEIGHT = 1.0f;
    private const float METER_ASSET_HEIGHT = 1.0f;

    private float _positiveHeight;
    private float _negativeHeight;


    public float ScalarValue
    {
      get { return _scalarValue; }
      set
      {
        _scalarValue = value;
      }
    }

    private void UpdateBackground()
    {
      float totalScaleSize = _max - _min;
      bool hasNegative = _min < 0;
      float positiveNormalized = hasNegative ? _max / totalScaleSize : 1.0f;
      float negativeNormalized = 1.0f - positiveNormalized;
      _positiveHeight = positiveNormalized * _height;
      _negativeHeight = -negativeNormalized * _height;
      float positiveScale = _positiveHeight / BACKGROUND_ASSET_HEIGHT;
      float negativeScale = _negativeHeight / BACKGROUND_ASSET_HEIGHT;
      float positivePosition = _positiveHeight / 2.0f;
      float negativePosition = _negativeHeight / 2.0f;

      _positiveBackground.SetLocalYScale(positiveScale);
      _negativeBackground.SetLocalYScale(negativeScale);
      _positiveBackground.SetLocalY(positivePosition);
      _negativeBackground.SetLocalY(negativePosition);
    }

    private void UpdateMeter()
    {
      float sign = ScalarValue >= 0 ? 1 : -1;
      bool hasPositive = _max >= 0;
      bool hasNegative = _min < 0;
      float normalizedValue = 0;
      float normalizeMax = 0;
      float normalizeMin = 0;

      if (hasPositive && hasNegative)
      {
        normalizeMax = sign > 0 ? _max : 0;
        normalizeMin = sign > 0 ? 0 : _min;
      }
      else
      {
        normalizeMax = _max;
        normalizeMin = _min;
      }

      normalizedValue = (_scalarValue - normalizeMin) / (normalizeMax - normalizeMin);

      normalizedValue = Mathf.Clamp01(normalizedValue);

      normalizedValue = sign > 0 ? normalizedValue : 1.0f - normalizedValue;

      float maxHeight = sign > 0 ? _positiveHeight : _negativeHeight;
      float newHeight = maxHeight * normalizedValue;
      float newPosition = newHeight / 2.0f;
      float newScale = newHeight / METER_ASSET_HEIGHT;

      _meter.SetLocalY(newPosition);
      _meterText.transform.SetLocalY(newHeight);
      _meter.SetLocalYScale(newScale);

      _meterText.text = ScalarValue.ToString("N2");
    }

    // Update is called once per frame
    void Update()
    {
      UpdateBackground();
      UpdateMeter();
    }
  }
}
