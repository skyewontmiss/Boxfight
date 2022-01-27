using UnityEngine;

namespace SlimUI.ModernMenu{
	[CreateAssetMenu(menuName = "ThemeSettings")]
	[System.Serializable]
	public class ThemeEditor : ScriptableObject {
		[System.Serializable]
		public class Custom1{
			[Header("Text")]	
			public Color graphic1;
			public Color32 text1;
		}

		[System.Serializable]
		public class Custom2{
			[Header("Text")]	
			public Color graphic2;
			public Color32 text2;
		}

		[System.Serializable]
		public class Custom3{
			[Header("Text")]	
			public Color graphic3;
			public Color32 text3;
		}

		[System.Serializable]
		public class Custom4
		{
			[Header("Text")]
			public Color graphic4;
			public Color32 text4;
		}

		[System.Serializable]
		public class Custom5
		{
			[Header("Text")]
			public Color graphic5;
			public Color32 text5;
		}


		[Header("CUSTOM SETTINGS")]
		public Custom1 NeonMarine;
		public Custom2 Midnight;
		public Custom3 Fluffy;
		public Custom4 BigContrast;
		public Custom5 LightMode;

		[HideInInspector]
		public Color currentColor;
		[HideInInspector]
		public Color32 textColor;
	}
}