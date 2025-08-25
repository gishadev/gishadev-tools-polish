using UnityEngine;

namespace gishadev.tools.Events
{
    [CreateAssetMenu(fileName = "StringEventChannelSO", menuName = "ScriptableObjects/Events/StringEventChannelSO")]
    public class StringEventChannelSO : EventChannelSO<StringWrapper>
    {
    }
    
    [System.Serializable]
    public struct StringWrapper
    {
        public string value;

        public StringWrapper(string value)
        {
            this.value = value;
        }
    }
}