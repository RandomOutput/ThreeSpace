﻿// Copyright 2017 Daniel Plemmons

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
using System.Collections;
using System.Collections.Generic;

namespace LineDrawing {
  public class Line {
    private Cylinder m_cylinder;
    private Vector3[] m_vertCache;

    public GameObject gameObject
    {
      get
      {
        if(m_cylinder == null)
        {
          return null;
        }

        return m_cylinder.gameObject;
      }
    }

    public Line(int vertexCount, int facesAround, float radius) {
      m_cylinder = Cylinder.MakeCylinder(facesAround, vertexCount - 2, radius);
      initializeVerts(vertexCount);
    }

    public Line(Vector3[] verticies, int facesAround, float radius) {
      m_cylinder = Cylinder.MakeCylinder(facesAround, verticies.Length - 2, radius);
      initializeVerts(verticies);
    }

    private void initializeVerts(int vertCount) {
      m_vertCache = new Vector3[vertCount];
      for (int i = 0; i < vertCount; i++) {
        m_vertCache[i] = i * Vector3.down;
      }
      updateVerts(m_vertCache);
    }

    private void initializeVerts(Vector3[] verts) {
      m_vertCache = verts;
      updateVerts(m_vertCache);
    }

    public Vector3 this[int vertex] {
      get {
        return m_cylinder[vertex].Position;
      }
      set {
        m_vertCache[vertex] = value;
        updateVerts(m_vertCache);
      }
    }

    public Vector3[] Verticies {
      get {
        return m_vertCache;
      }
    }

    public void SetVerticies(Vector3[] verts) {
      m_vertCache = verts;
      updateVerts(m_vertCache);
    }

    private void updateVerts(Vector3[] verticies) {
      if (verticies.Length != m_cylinder.Count) {
        Debug.Log("verts: " + verticies.Length + " | cyl rings: " + m_cylinder.Count);
        throw new System.ArgumentOutOfRangeException();
        // TODO: Update this to just replace the mesh.
      }

      // Special case for first vert
      var initialDirection = (verticies[1] - verticies[0]).normalized;
      m_cylinder[0].Position = verticies[0];
      m_cylinder[0].SetNormal(initialDirection);

      for (int i = 1; i < verticies.Length; i++) {
        var inDirection = verticies[i] - verticies[i - 1];
        var outDirection = i == verticies.Length - 1 ? inDirection : verticies[i + 1] - verticies[i];
        var averageDirection = ((inDirection + outDirection) / 2.0f).normalized;

        Vector3 lastRootPosition = m_cylinder[i - 1].Position + m_cylinder[i - 1].ToRootVertex;
        Vector3 fromLastPoint = verticies[i] - verticies[i - 1];
        Vector3 projectionRayFromLastRoot = RingTransform.PlaneProjectionAlongVector(verticies[i], averageDirection, lastRootPosition, fromLastPoint);
        Vector3 lastRootProjectedToRingPlane = lastRootPosition + projectionRayFromLastRoot;
        Vector3 centerToLastRootProjection = lastRootProjectedToRingPlane - verticies[i];
        Vector3 projectedDirectionToNextRoot = centerToLastRootProjection.normalized;

        m_cylinder[i].Position = verticies[i];
        m_cylinder[i].SetNormal(averageDirection, projectedDirectionToNextRoot);
      }
    }
  }
}
