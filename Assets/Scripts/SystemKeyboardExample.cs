﻿// Copyright (c) Mixed Reality Toolkit Contributors
// Licensed under the BSD 3-Clause

// Disable "missing XML comment" warning for samples. While nice to have, this XML documentation is not required for samples.

#pragma warning disable CS1591

using MixedReality.Toolkit.Input;
using MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;

namespace MixedReality.Toolkit.Examples.Demos
{
    /// <summary>
    /// An example script that delegates keyboard API access either to the WinRT <c>MixedRealityKeyboard</c> class
    /// or Unity's <see cref="TouchScreenKeyboard"/> class depending on the platform.
    /// </summary>
    /// <remarks>
    /// <para>Note that like Unity's TouchScreenKeyboard API, this script only supports WSA, iOS, and Android.</para>
    /// </remarks>
    [AddComponentMenu("MRTK/Examples/System Keyboard Example")]
    public class SystemKeyboardExample : MonoBehaviour
    {
#if WINDOWS_UWP
        private WindowsMRKeyboard wmrKeyboard;
#elif UNITY_IOS || UNITY_ANDROID
        private TouchScreenKeyboard touchscreenKeyboard;
#endif

        [SerializeField] private TextMeshPro debugMessage = null;

#pragma warning disable 0414
        [SerializeField] private KeyboardPreview mixedRealityKeyboardPreview = null;
#pragma warning restore 0414

        /// <summary>
        /// Opens a platform specific keyboard.
        /// </summary>
        public void OpenSystemKeyboard()
        {
#if WINDOWS_UWP
            wmrKeyboard.ShowKeyboard(wmrKeyboard.Text, false);
#elif UNITY_IOS || UNITY_ANDROID
            touchscreenKeyboard = TouchScreenKeyboard.Open(string.Empty, TouchScreenKeyboardType.Default, false, false,
                false, false);
#endif
        }

        #region MonoBehaviour Implementation

        /// <summary>
        /// A Unity event function that is called on the frame when a script is enabled just before any of the update methods are called the first time.
        /// </summary> 
        private void Start()
        {
            // Initially hide the preview.
            if (mixedRealityKeyboardPreview != null)
            {
                mixedRealityKeyboardPreview.gameObject.SetActive(false);
            }

#if WINDOWS_UWP
            // Windows mixed reality keyboard initialization goes here
            wmrKeyboard = gameObject.AddComponent<WindowsMRKeyboard>();
            if (wmrKeyboard.OnShowKeyboard != null)
            {
                wmrKeyboard.OnShowKeyboard.AddListener(() =>
                {
                    if (mixedRealityKeyboardPreview != null)
                    {
                        mixedRealityKeyboardPreview.gameObject.SetActive(true);
                    }
                });
            }

            if (wmrKeyboard.OnHideKeyboard != null)
            {
                wmrKeyboard.OnHideKeyboard.AddListener(() =>
                {
                    if (mixedRealityKeyboardPreview != null)
                    {
                        mixedRealityKeyboardPreview.gameObject.SetActive(false);
                    }
                });
            }
#elif UNITY_IOS || UNITY_ANDROID
            // non-Windows mixed reality keyboard initialization goes here
#else
            debugMessage.text = "Keyboard not supported on this platform.";
#endif
        }


#if WINDOWS_UWP
        /// <summary>
        /// A Unity event function that is called every frame, if this object is enabled.
        /// </summary>
        private void Update()
        {
            // Windows mixed reality keyboard update goes here
            if (wmrKeyboard.Visible)
            {
                if (debugMessage != null)
                {
                     debugMessage.text += wmrKeyboard.Text + "\n"; 
                }

                if (mixedRealityKeyboardPreview != null)
                {
                    mixedRealityKeyboardPreview.Text = wmrKeyboard.Text;
                    mixedRealityKeyboardPreview.CaretIndex = wmrKeyboard.CaretIndex;
                }
            }
            else
            {
                var keyboardText = wmrKeyboard.Text;

                if (string.IsNullOrEmpty(keyboardText))
                {
                    if (debugMessage != null)
                    {
                        debugMessage.text = "Open keyboard to type text.";
                    }
                }
                else
                {
                    if (debugMessage != null)
                    {
                       debugMessage.text +=  keyboardText + "\n";
                    }
                }

                if (mixedRealityKeyboardPreview != null)
                {
                    mixedRealityKeyboardPreview.Text = string.Empty;
                    mixedRealityKeyboardPreview.CaretIndex = 0;
                }
            }
        }
#elif UNITY_IOS || UNITY_ANDROID
        /// <summary>
        /// A Unity event function that is called every frame, if this object is enabled.
        /// </summary>
        private void Update()
        {
            // non-Windows mixed reality keyboard initialization goes here
            // for non-Windows mixed reality keyboards just use Unity's default
            // touch screen keyboard.
            if (touchscreenKeyboard != null)
            {
                string KeyboardText = touchscreenKeyboard.text;
                if (TouchScreenKeyboard.visible)
                {
                    if (debugMessage != null)
                    {
                        debugMessage.text +=  KeyboardText + "\n";
                    }
                }
                else
                {
                    if (debugMessage != null)
                    {
                        debugMessage.text +=  KeyboardText + "\n";
                    }

                    touchscreenKeyboard = null;
                }
            }
        }
#endif

        #endregion MonoBehaviour Implementation
    }
}
#pragma warning restore CS1591