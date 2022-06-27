using System;

using UnityEngine;
using UnityEngine.UI;

namespace MixChatter.Scripts.Components
{
    // TODO: refactor. Use IComponent to handle component in game system
    public class ChatComponent : MonoBehaviour
    {
        public Text UsernameText => _UsernameText
            ? _UsernameText
            : throw new NullReferenceException($"Unable to find component {nameof(_UsernameText)}");

        public Text MessageText => _MessageText
            ? _MessageText
            : throw new NullReferenceException($"Unable to find component {nameof(_MessageText)}");

        [SerializeField]
        private Text _UsernameText;

        [SerializeField]
        private Text _MessageText;
    }
}
