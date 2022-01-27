using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace SlimUI.ModernMenu
{
	public class ThemeController : MonoBehaviour
	{


		public enum Theme {custom1, custom2, custom3, custom4, custom5};
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
			if(theme == Theme.custom1)
			{
				themeController.currentColor = themeController.NeonMarine.graphic1;
				themeController.textColor = themeController.NeonMarine.text1;
				themeIndex = 0;
			}
			else if(theme == Theme.custom2)
			{
				themeController.currentColor = themeController.Midnight.graphic2;
				themeController.textColor = themeController.Midnight.text2;
				themeIndex = 1;
			}
			else if(theme == Theme.custom3)
			{
				themeController.currentColor = themeController.Fluffy.graphic3;
				themeController.textColor = themeController.Fluffy.text3;
				themeIndex = 2;
			}
			else if (theme == Theme.custom4)
			{
				themeController.currentColor = themeController.BigContrast.graphic4;
				themeController.textColor = themeController.BigContrast.text4;
				themeIndex = 3;
			}
			else if (theme == Theme.custom5)
			{
				themeController.currentColor = themeController.LightMode.graphic5;
				themeController.textColor = themeController.LightMode.text5;
				themeIndex = 4;
			}
		}

		public void UpdateThemeColorsAtRuntime(int theme)
		{
			if (theme == 0)
			{
				themeController.currentColor = themeController.NeonMarine.graphic1;
				themeController.textColor = themeController.NeonMarine.text1;
				themeIndex = 0;
			}
			else if (theme == 1)
			{
				themeController.currentColor = themeController.Midnight.graphic2;
				themeController.textColor = themeController.Midnight.text2;
				themeIndex = 1;
			}
			else if (theme == 2)
			{
				themeController.currentColor = themeController.Fluffy.graphic3;
				themeController.textColor = themeController.Fluffy.text3;
				themeIndex = 2;
			}
			else if (theme == 3)
			{
				themeController.currentColor = themeController.BigContrast.graphic4;
				themeController.textColor = themeController.BigContrast.text4;
				themeIndex = 3;
			}
			else if (theme == 4)
			{
				themeController.currentColor = themeController.LightMode.graphic5;
				themeController.textColor = themeController.LightMode.text5;
				themeIndex = 4;
			}
		}
	}
}