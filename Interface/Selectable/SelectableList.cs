using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

namespace Interface.Selectable {
	public abstract class SelectionList<TData, TComponent> : MonoBehaviour where TComponent : SelectionItem<TData> {
		#region Classes
			private class SelectionData {
				public TData data { get; private set; }
				public TComponent item { get; private set; }
				
				private bool _isSelected = false;
				public bool isSelected {
					get {
						return _isSelected;
					}
					set {
						if (_isSelected == value) {
							return;
						}
						
						_isSelected = value;
						
						// Fire item events.
						if (_isSelected) {
							item.onSelected.Invoke();
						} else {
							item.onDeselected.Invoke();
						}
					}
				}
				
				public SelectionData(TData _data, TComponent _item, bool _isSelected = false) {
					data = _data;
					item = _item;
					
					this._isSelected = !_isSelected;
					isSelected = _isSelected;
					
					_item.SetData(_data);
				}
			}
			public class EventTyped : UnityEvent<TData> {}
		#endregion
		
		#region Variables
			protected bool forceOne = true;
			protected bool allowNone = false;
			
			[SerializeField]
			private GameObject _selectionItemPrefab = default;
			
			private int _selectionCount = 0;
			private List<SelectionData> _selectionDataList = new List<SelectionData>();
		#endregion
		
		#region MonoBehaviour functions
			private void OnEnable() {
				foreach (SelectionData _selectionData in _selectionDataList) {
					if (_selectionData.isSelected) {
						_selectionData.item.onSelected.Invoke();
					} else {
						_selectionData.item.onDeselected.Invoke();
					}
				}
			}
		#endregion
		
		#region Event handlers
			private void OnClick(Transform _transform) {
				// Get data.
				TComponent _item = _transform.GetComponent<TComponent>();
				SelectionData _selectionData;
				if (!DataTryGet(_item, out _selectionData)) {
					return;
				}
				
				if (!allowNone && _selectionData.isSelected && _selectionCount <= 1) {
					// Cannot deselect the last selected item.
					return;
				}
				
				// Toggle is selected.
				_selectionData.isSelected = !_selectionData.isSelected;
				
				// If only one is allowed to be selected.
				if (forceOne && _selectionData.isSelected) {
					// Deselect all others.
					foreach (SelectionData _otherData in _selectionDataList) {
						if (_otherData.isSelected && _selectionData.item != _otherData.item) {
							_otherData.isSelected = false;
							_selectionCount--;
						}
					}
				}
				
				if (_selectionData.isSelected) {
					_selectionCount++;
					OnSelectionAdded(_selectionData.data);
				} else {
					_selectionCount--;
					OnSelectionRemoved(_selectionData.data);
				}
			}
			
			protected virtual void OnSelectionAdded(TData _data) {}
			protected virtual void OnSelectionRemoved(TData _data) {}
		#endregion
		
		#region Protected functions
			protected bool SpawnItem(TData _data) {
				if (DataContains(_data)) {
					return false;
				}
				
				SpawnItemUnsafe(_data);
				return true;
			}
			protected void SpawnItems(params TData[] _dataSet) {
				_dataSet = DataFilter(_dataSet.ToList(), true).ToArray();
				foreach (TData _data in _dataSet) {
					SpawnItemUnsafe(_data);
				}
			}
			protected bool DestroyItem(TData _data) {
				SelectionData _selectionData;
				if (!DataTryGet(_data, out _selectionData)) {
					return false;
				}
				
				_selectionDataList.Remove(_selectionData);
				_selectionData.item.onClick.RemoveListener(OnClick);
				Destroy(_selectionData.item.gameObject);
				return true;
			}
			protected void DestroyItems(params TData[] _dataSet) {
				foreach (TData _data in _dataSet) {
					DestroyItem(_data);
				}
			}
			
			protected bool AddToSelection(TData _data) {
				SelectionData _selectionData;
				if (!DataTryGet(_data, out _selectionData)) {
					return false;
				}
				
				if (!_selectionData.isSelected) {
					OnClick(_selectionData.item.transform);
				}
				
				return true;
			}
			protected bool RemoveFromSelection(TData _data) {
				SelectionData _selectionData;
				if (!DataTryGet(_data, out _selectionData)) {
					return false;
				}
				
				if (_selectionData.isSelected) {
					OnClick(_selectionData.item.transform);
				}
				
				return true;
			}
		#endregion
		
		#region Private functions
			private bool DataContains(TComponent _item) {
				foreach (SelectionData _selectionDataItem in _selectionDataList) {
					if (_selectionDataItem.item == _item) {
						return true;
					}
				}
				
				return false;
			}
			private bool DataContains(TData _data) {
				foreach (SelectionData _selectionDataItem in _selectionDataList) {
					if (_selectionDataItem.data.Equals(_data)) {
						return true;
					}
				}
				
				return false;
			}
			
			private bool DataTryGet(TComponent _item, out SelectionData _selectionData) {
				foreach (SelectionData _selectionDataItem in _selectionDataList) {
					if (_selectionDataItem.item == _item) {
						_selectionData = _selectionDataItem;
						return true;
					}
				}
				
				_selectionData = default;
				return false;
				
			}
			private bool DataTryGet(TData _data, out SelectionData _selectionData) {
				foreach (SelectionData _selectionDataItem in _selectionDataList) {
					if (_selectionDataItem.data.Equals(_data)) {
						_selectionData = _selectionDataItem;
						return true;
					}
				}
				
				_selectionData = default;
				return false;
			}
			
			private List<TData> DataFilter(List<TData> _dataSet, bool _doRemoveExisting) {
				foreach (SelectionData _selectionData in _selectionDataList) {
					for (int _i = 0; _i < _dataSet.Count; _i++) {
						if (_dataSet[_i].Equals(_selectionData.data) == _doRemoveExisting) {
							_dataSet.RemoveAt(_i);
						}
					}
				}
				return _dataSet;
			}
			
			private void SpawnItemUnsafe(TData _data) {
				TComponent _item = Instantiate(_selectionItemPrefab, transform).GetComponent<TComponent>();
				if (_item == default) {
					Log.ErrorFormat("Prefab of '{0}' does not contain a component inheriting from 'SelectionItem<T>'.", gameObject.name);
					return;
				}
				
				_item.onClick.AddListener(OnClick);
				_selectionDataList.Add(new SelectionData(_data, _item));
			}
		#endregion
	}
}