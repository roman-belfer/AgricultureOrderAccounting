using Argo.DataAccess.All.Models;
using Argo.DataAccess.LaborDetail.Model;
using FMS.DataManagers.Interfaces;
using FMS.Services.NetTcpDirectoryServiceReference;
using OrderAccounting.Common.Infrastructure.Interfaces;
using OrderAccounting.Common.Infrastructure.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels
{
    public class ObjectItemViewModel : BaseDataItemViewModel, IOrderItemViewModel
    {
        #region Properties

        private bool _isSingleObject;
        public bool IsSingleObject
        {
            get { return _isSingleObject; }
            set
            {
                if (_isSingleObject != value)
                {
                    _isSingleObject = value;
                    RaisePropertyChanged("IsSingleObject");
                }
            }
        }

        private ObservableCollection<ObjectModel> _objectCollection;
        public ObservableCollection<ObjectModel> ObjectCollection
        {
            get { return _objectCollection; }
            set
            {
                if (value != null)
                {
                    if (_objectCollection != value)
                    {
                        _objectCollection = value;
                        _objectCollection.CollectionChanged += ObjectCollectionChanged;
                    }
                }
            }
        }

        private object _objectFrom;
        public object ObjectFrom
        {
            get { return _objectFrom; }
            set
            {
                if (_objectFrom != value)
                {
                    _objectFrom = value;
                    RaisePropertyChanged("ObjectFrom");
                }
            }
        }

        private object _objectTo;
        public object ObjectTo
        {
            get { return _objectTo; }
            set
            {
                if (_objectTo != value)
                {
                    _objectTo = value;
                    RaisePropertyChanged("ObjectTo");
                }
            }
        }

        #endregion

        #region Initialization

        public ObjectItemViewModel(IDirectoryManager directoryManager) : base(directoryManager)
        {
            Title = "Облікові об'єкти";

            IsSingleObject = true;

            ObjectTo = null;
            ObjectFrom = null;

            ObjectCollection = new ObservableCollection<ObjectModel>();
        }

        #endregion

        #region Event Executors

        private void ObjectCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnDataChanged();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns validation bool value if objects data is not empty
        /// </summary>
        public override bool IsDataValid()
        {
            IsValid = true;

            IsValid = ObjectCollection != null && ObjectCollection.Count > 0 && ObjectCollection.All(x => x.IsValid());

            return IsValid;
        }

        /// <summary>
        /// Check is input objects' pair was not added to the collection before
        /// </summary>
        private bool CheckDoubleObjectInCollection(out string errorMessage)
        {
            errorMessage = "Дана пара об'єктів вже зарєєстрована у наряді!";

            ObjectModel objectFrom, objectTo;

            GeneratoFromToObjects(out objectFrom, out objectTo);

            if (objectFrom == null || objectTo == null)
            {
                errorMessage = "Об'єкт відбуття та об'єкт прибуття повинні бути обрані!";
                return false;
            }

            if (objectFrom.Equals(objectTo))
            {
                errorMessage = "Об'єкт прибуття та об'єкт відбуття не можуть бути однаковими!";
                return false;
            }

            return ObjectCollection.Count == 0 || ObjectCollection.All(x => x.IsInputDoubleObjectValid(objectFrom, objectTo));
        }

        /// <summary>
        /// Adding single object to the collection depending on type
        /// </summary>
        private void AddSingleObject(object obj)
        {
            if (obj is OutlinesNetTcp)
            {
                ObjectCollection.Add(ObjectModel.ConvertToModel(obj as OutlinesNetTcp, _directoryManager));
            }
            else if (obj is CropPlan.ListItem)
            {
                ObjectCollection.Add(ObjectModel.ConvertToModel(obj as CropPlan.ListItem, _directoryManager));
            }
            else if (obj is FieldVersion.ListItem)
            {
                ObjectCollection.Add(ObjectModel.ConvertToModel(obj as FieldVersion.ListItem, _directoryManager));
            }
        }

        /// <summary>
        /// Adding objects' pair to the collection
        /// </summary>
        private void AddDoubleObject()
        {
            ObjectModel objectFrom, objectTo;

            GeneratoFromToObjects(out objectFrom, out objectTo);

            if (objectFrom == null || objectTo == null) return;

            ObjectCollection.Add(ObjectModel.ConvertToModel(objectFrom, objectTo));
        }

        /// <summary>
        /// Generating objects for objects' pair before adding to the collection
        /// </summary>
        private void GeneratoFromToObjects(out ObjectModel objectFrom, out ObjectModel objectTo)
        {
            objectFrom = null;
            objectTo = null;

            if (ObjectFrom is FieldVersion.ListItem)
            {
                objectFrom = ObjectModel.ConvertToModel(ObjectFrom as FieldVersion.ListItem, _directoryManager);
            }
            else if (ObjectFrom is OutlinesNetTcp)
            {
                objectFrom = ObjectModel.ConvertToModel(ObjectFrom as OutlinesNetTcp, _directoryManager);
            }

            if (ObjectTo is FieldVersion.ListItem)
            {
                objectTo = ObjectModel.ConvertToModel(ObjectTo as FieldVersion.ListItem, _directoryManager);
            }
            else if (ObjectTo is OutlinesNetTcp)
            {
                objectTo = ObjectModel.ConvertToModel(ObjectTo as OutlinesNetTcp, _directoryManager);
            }
        }

        private bool IsSingleObjectValid(object obj)
        {
            if (obj is OutlinesNetTcp)
            {
                var outline = obj as OutlinesNetTcp;

                return CanAddObjectToCollection(outline);
            }
            else if (obj is CropPlan.ListItem)
            {
                var outline = obj as CropPlan.ListItem;

                return CanAddObjectToCollection(outline);
            }
            else if (obj is FieldVersion.ListItem)
            {
                var outline = obj as FieldVersion.ListItem;

                return CanAddObjectToCollection(outline);
            }

            return false;
        }

        /// <summary>
        /// Check is input object was not added to the collection before
        /// </summary>
        private bool CanAddObjectToCollection<T>(T outline)
        {
            return ObjectCollection.Count == 0 || ObjectCollection.All(x => x.IsInputObjectValid(outline));
        }

        public override void ConvertDataToDTO<T>(T item)
        {
            if (typeof(T) == typeof(LaborDetail.Item))
            {
                var model = (item as LaborDetail.Item);

                model.Objects = new List<LaborDetailObject.Item>();
                foreach (var obj in ObjectCollection)
                {
                    LaborDetailObject.Item objdb = new LaborDetailObject.Item();
                    model.Objects.Add(objdb);

                    objdb.Identity = obj.LaborDetailObjectID;
                    objdb.Loading = obj.ObjectId.HasValue ? new List<LaborDetailObjectItem.Item>() { new LaborDetailObjectItem.Item() { Identity = obj.LaborDetailObjectItemFromID, Outline = new Argo.DataAccess.All.Models.Outline.ListItem() { Identity = obj.ObjectId.Value } } } : null;

                    if (!IsSingleObject)
                    {
                        objdb.Unloading = obj.ToObjectId.HasValue ? new List<LaborDetailObjectItem.Item>() { new LaborDetailObjectItem.Item() { Identity = obj.LaborDetailObjectItemToID, Outline = new Argo.DataAccess.All.Models.Outline.ListItem() { Identity = obj.ToObjectId.Value } } } : null;
                    }
                }
            }
        }

        public override void ConvertDataFromDTO<T>(T item)
        {
            if (typeof(T) == typeof(LaborDetail.Item))
            {
                var model = (item as LaborDetail.Item);

                ObjectCollection = new ObservableCollection<ObjectModel>();
                foreach (var obj in model.Objects)
                {
                    int Year = 0;
                    if (model.ActualPhaseYear != null)
                    {
                        int.TryParse(model.ActualPhaseYear.DisplayName, out Year);
                    }

                    var objModel = ObjectModel.ConvertToModelFromLaborDetailObject(obj, Year, _directoryManager);
                    if (objModel != null)
                    {
                        objModel.LaborDetailObjectID = obj.Identity;
                        ObjectCollection.Add(objModel);

                        if (obj.Unloading.Count > 0)
                        {
                            IsSingleObject = false;
                        }
                        else
                        {
                            IsSingleObject = true;
                        }
                    }
                }
            }
        }

        public override int? GetOperationId()
        {
            return null;
        }

        public override int? GetBaseOperationId()
        {
            return null;
        }

        public override int? GetPhaseId()
        {
            return null;
        }

        public override int? GetOperationTypeId()
        {
            return null;
        }

        public override void SetObjectData(bool isTransportation)
        {
            IsSingleObject = !isTransportation;
        }

        #endregion

        #region Command Executors

        protected override void RemoveCommandExecute(object obj)
        {
            if (!IsRemoveCommited())
            {
                return;
            }

            ObjectCollection.Remove(obj as ObjectModel);
        }

        protected override void AddCommandExecute(object obj)
        {
            if (IsSingleObject)
            {
                AddSingleObject(obj);
            }
            else
            {
                string errorMessage;

                if (!CheckDoubleObjectInCollection(out errorMessage))
                {
                    MessageBox.Show(errorMessage, "Не вдалось додати об'єкти!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                AddDoubleObject();
            }
        }

        protected override bool AddCommandCanExecute(object obj)
        {
            bool canExecute = false;

            if (IsSingleObject)
            {
                canExecute = IsSingleObjectValid(obj);
            }
            else
            {
                canExecute = ObjectFrom != null && ObjectTo != null;
            }

            return canExecute;
        }

        #endregion

    }
}
