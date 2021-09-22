using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

public interface IRaycastable {
    CursorType GetCursorType();
    bool HandleRaycast(PlayerController callingController);
}
