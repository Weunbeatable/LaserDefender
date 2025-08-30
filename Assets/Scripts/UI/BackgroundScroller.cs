using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD.UI
{
    public class BackgroundScroller : MonoBehaviour
    {
        [SerializeField] float _background_Scroll_Speed = 0.5f;
        Material _myMaterial;
        Vector2 _offset;
        [SerializeField] Texture[] _possibleBackgrounds;
        float _swap_Background_Timer;
        int _current_Background_Index;
        [SerializeField] float _min_Background_Time;
        [SerializeField] float _max_Background_Time;
        // Start is called before the first frame update
        void Start()
        {
            _myMaterial = GetComponent<Renderer>().material;
            _offset = new Vector2(0f, _background_Scroll_Speed);
            // Inital timer set two swap the background 
            _swap_Background_Timer = UnityEngine.Random.Range(_min_Background_Time, _max_Background_Time);
        }

        // Update is called once per frame
        void Update()
        {
            _myMaterial.mainTextureOffset += _offset * Time.deltaTime; // maintexture offset allows us to access the offset variable, and we multiply offset by delta time to be framerate independent
            //_myMaterial.mainTexture
            _swap_Background_Timer -= Time.deltaTime;
            if (_swap_Background_Timer <= 0) // once the timer reaches 0 implement function to update the mainTexture on the renderer. 
            {
                _ChangeBackground();
            }
        }

        private void _ChangeBackground()
        {
           int _index = UnityEngine.Random.Range(0 , _possibleBackgrounds.Length);
            if (_index == _current_Background_Index)
            {
                _index = UnityEngine.Random.Range(0, _possibleBackgrounds.Length);
            }
            _current_Background_Index = _index;
            GetComponent<Renderer>().material.mainTexture = _possibleBackgrounds[ _current_Background_Index ];
            _swap_Background_Timer = UnityEngine.Random.Range(_min_Background_Time, _max_Background_Time);
        }
        // Smol update scrolling through different  backgorunds over a period of time just for added visual enjoyment. 
    }
}