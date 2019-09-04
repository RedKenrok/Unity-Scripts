using UnityEngine.UI;

namespace Interface {
	public class NonDrawingGraphic : MaskableGraphic {
		public override void SetMaterialDirty() {
			return;
		}
		public override void SetVerticesDirty() {
			return;
		}
		
		protected override void OnPopulateMesh(VertexHelper _vertexHelper) {
			_vertexHelper.Clear();
			return;
		}
	}
}