using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modular approach to simple interactions. Prevents the need for excessive if statements/tags/layers/string lookups/etc and can allows for easy to create interactions in the world.
/// 
/// If you have any questions for how the system works, feel free to ask. I will have some example prefabs displaying how the systems work.
/// 
/// I am realizing that the name here isn't the best, potentially will rename...
/// </summary>
[CreateAssetMenu(fileName = "InteractableObject", menuName = "InteractableObject")]
public class InterableObject : ScriptableObject
{
    [Tooltip("The InteractableObjects with which this object can interact with. Allows us to make objects in the world that are only interactable " +
        "with certain objects/characters (ring of fire only lets red through) or while those characters are in certain states (ice slam breaks floor)." +
        "The burden should be placed on the object, not the thing interacting with it. For example a button that can only be interacted with by BigRed should" +
        "have an InteractableObject with BigRed in its InteractsWith list.")]
    public List<InterableObject> interactsWith = new List<InterableObject>();
}
