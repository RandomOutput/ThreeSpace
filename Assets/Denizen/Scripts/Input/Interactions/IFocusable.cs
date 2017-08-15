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

using System;
using UnityEngine;

namespace Denizen.Input.Interactions
{
  public interface IFocusable
  {
    event Action Focused;
    event Action Unfocused;

    GameObject ParentObject
    {
      get;
    }

    bool IsFocused
    {
      get;
    }

    IFocuser Focuser
    {
      get;
    }

    bool TryFocus(IFocuser focuser);
    bool TryUnfocus(IFocuser focuser);
  }
}
