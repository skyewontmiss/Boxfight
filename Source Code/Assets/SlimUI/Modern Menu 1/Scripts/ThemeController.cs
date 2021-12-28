using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace SlimUI.ModernMenu
{
	public class ThemeController : MonoBehaviour
	{


		public enum Theme {custom1, custom2, custom3};
		[Header("Theme Settings")]
		public Theme theme;
		int themeIndex;
		public ThemeEditor themeController;

		void Awake()
		{
			SetThemeColors();
		}

		void SetThemeColors()
		{
			if(theme == Theme.custom1){
				themeController.currentColor = themeController.custom1.graphic1;
				themeController.textColor = themeController.custom1.text1;
				themeIndex = 0;
			}else if(theme == Theme.custom2){
				themeController.currentColor = themeController.custom2.graphic2;
				themeController.textColor = themeController.custom2.text2;
				themeIndex = 1;
			}else if(theme == Theme.custom3){
				themeController.currentColor = themeController.custom3.graphic3;
				themeController.textColor = themeController.custom3.text3;
				themeIndex = 2;
			}
		}
	}
}