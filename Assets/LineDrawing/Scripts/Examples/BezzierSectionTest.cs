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

using UnityEngine;
using LineDrawing;

public class BezzierSectionTest : MonoBehaviour {
  public int m_pointCount;
  public Transform m_p0;
  public Transform m_p1;
  public Transform m_p2;
  public Transform m_p3;

  private BezzierSection m_bezzierSection;

	// Use this for initialization
	void Start () {
    m_bezzierSection = new BezzierSection(m_pointCount, m_p0.position, m_p1.position, m_p2.position, m_p3.position);
	}

	// Update is called once per frame
  void Update () {
    m_bezzierSection[0] = m_p0.position;
    m_bezzierSection[1] = m_p1.position;
    m_bezzierSection[2] = m_p2.position;
    m_bezzierSection[3] = m_p3.position;
	}
}
