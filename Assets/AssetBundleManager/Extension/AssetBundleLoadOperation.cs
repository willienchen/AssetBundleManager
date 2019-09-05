﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using System.Linq;
//using AgileSlot.Data;
#endif

namespace AssetBundles {

    public abstract class AssetBundleLoadOperation : IEnumerator {

        public object Current => null;

        public bool MoveNext() => !IsDone();

        public void Reset() { }

        abstract public bool IsDone();

        abstract public bool IsError();
    }
}