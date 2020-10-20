using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HW.UI
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField]
        List<TMP_Text> instructions;

        [SerializeField]
        [TextArea(1,3)]
        List<string> it;

        [SerializeField]
        [TextArea(1, 3)]
        List<string> en;

        private void Awake()
        {
            List<string> insts = GetInstructions(GameManager.Instance.Language);

            for(int i=0; i<insts.Count; i++)
            {
                instructions[i].text = insts[i];
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        List<string> GetInstructions(Language language)
        {
            List<string> ret;

            switch (language)
            {
                case Language.Italian:
                    ret = it;
                    break;
                case Language.English:
                    ret = en;
                    break;

                default:
                    ret = en;
                    break;
            }

            return ret;
        }
    }

}
