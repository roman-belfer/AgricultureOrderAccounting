using Argo.DataAccess.All.Models;
using Argo.DataAccess.LaborDetail.Model;
using FMS.DataManagers.Interfaces;
using FMS.Services.NetTcpDirectoryServiceReference;
using Prism.Mvvm;
using System.Linq;

namespace OrderAccounting.Common.Infrastructure.Models
{
    public class ObjectModel : BindableBase
    {
        #region Properties

        public int LaborDetailObjectID { get; set; }
        public int LaborDetailObjectItemFromID { get; set; }
        public int LaborDetailObjectItemToID { get; set; }

        private int? _objectId;
        public int? ObjectId
        {
            get { return _objectId; }
            set
            {
                if (_objectId != value)
                {
                    _objectId = value;
                    RaisePropertyChanged("ObjectId");
                }
            }
        }

        private int? _objectTypeId;
        public int? ObjectTypeId
        {
            get { return _objectTypeId; }
            set
            {
                if (_objectTypeId != value)
                {
                    _objectTypeId = value;
                    RaisePropertyChanged("ObjectTypeId");
                }
            }
        }

        private string _displayFrom;
        public string DisplayFrom
        {
            get { return _displayFrom; }
            private set
            {
                if (_displayFrom != value)
                {
                    _displayFrom = value;
                    RaisePropertyChanged("DisplayFrom");
                }
            }
        }

        private int? _toObjectId;
        public int? ToObjectId
        {
            get { return _toObjectId; }
            set
            {
                if (_toObjectId != value)
                {
                    _toObjectId = value;
                    RaisePropertyChanged("ToObjectId");
                }
            }
        }

        private int? _toObjectTypeId;
        public int? ToObjectTypeId
        {
            get { return _toObjectTypeId; }
            set
            {
                if (_toObjectTypeId != value)
                {
                    _toObjectTypeId = value;
                    RaisePropertyChanged("ToObjectTypeId");
                }
            }
        }

        private string _displayTo;
        public string DisplayTo
        {
            get { return _displayTo; }
            private set
            {
                if (_displayTo != value)
                {
                    _displayTo = value;
                    RaisePropertyChanged("DisplayTo");
                }
            }
        }

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

        private bool _isCropPlan;
        public bool IsCropPlan
        {
            get { return _isCropPlan; }
            set
            {
                if (_isCropPlan != value)
                {
                    _isCropPlan = value;
                    RaisePropertyChanged("IsCropPlan");
                }
            }
        }

        #endregion

        #region Methods

        internal bool IsValid()
        {
            bool isValid;
            if (IsSingleObject)
            {
                isValid = ObjectId.HasValue;
            }
            else
            {
                isValid = ObjectId.HasValue && ToObjectId.HasValue;
            }

            return isValid;
        }

        /// <summary>
        /// Check is input object is not equal to current model
        /// </summary>
        public bool IsInputObjectValid<T>(T outline)
        {
            if (typeof(T) == typeof(CropPlan.ListItem))
            {
                return IsInputCropPlanValid(outline as CropPlan.ListItem);
            }
            else if (typeof(T) == typeof(OutlinesNetTcp))
            {
                return IsInputOutlineValid(outline as OutlinesNetTcp);
            }
            else if (typeof(T) == typeof(FieldVersion.ListItem))
            {
                return IsInputFieldValid(outline as FieldVersion.ListItem);
            }

            return false;
        }

        /// <summary>
        /// Check is input field object is not equal to current model
        /// </summary>
        private bool IsInputFieldValid(FieldVersion.ListItem fieldVersion)
        {
            if (IsCropPlan)
            {
                return true;
            }
            else
            {
                return ObjectTypeId == 8 ? ObjectId != fieldVersion.FVersOutlineId : true;
            }
        }

        /// <summary>
        /// Check is input outline object is not equal to current model
        /// </summary>
        private bool IsInputOutlineValid(OutlinesNetTcp outlinesNetTcp)
        {
            if (IsCropPlan)
            {
                return true;
            }
            else
            {
                if (ObjectTypeId != 8)
                {
                    return ObjectTypeId == outlinesNetTcp.OutObjectTypeId ? ObjectId != outlinesNetTcp.OutId : true;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Check is input crop plan is not equal to current model
        /// </summary>
        private bool IsInputCropPlanValid(CropPlan.ListItem cropPlan)
        {
            if (IsCropPlan)
            {
                return ObjectId != cropPlan.OutlineID;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Check is input objects' pair is not equal to current model
        /// </summary>
        public bool IsInputDoubleObjectValid(ObjectModel objectFrom, ObjectModel objectTo)
        {
            bool isValid;
            if (ObjectTypeId == objectFrom.ObjectTypeId)
            {
                isValid = ObjectId != objectFrom.ObjectId;
            }
            else
            {
                isValid = true;
            }

            if (ToObjectTypeId == objectTo.ObjectTypeId)
            {
                isValid |= ToObjectId != objectTo.ObjectId;
            }
            else
            {
                isValid |= true;
            }

            return isValid;
        }

        public override bool Equals(object obj)
        {
            if (obj is ObjectModel)
            {
                var inputModel = obj as ObjectModel;

                return inputModel.ObjectTypeId == ObjectTypeId && inputModel.ObjectId == ObjectId &&
                    inputModel.ToObjectId == ToObjectId && inputModel.ToObjectTypeId == ToObjectTypeId;
            }

            return false;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////

        internal static ObjectModel ConvertToModel(ObjectModel objectFrom, ObjectModel objectTo)
        {
            var result = new ObjectModel
            {
                LaborDetailObjectID = objectFrom.LaborDetailObjectID,
                LaborDetailObjectItemFromID = objectFrom.LaborDetailObjectItemFromID,
                LaborDetailObjectItemToID = objectTo.LaborDetailObjectItemToID,
                IsSingleObject = false,
                ObjectId = objectFrom.ObjectId,
                ObjectTypeId = objectFrom.ObjectTypeId,
                ToObjectId = objectTo.ObjectId,
                ToObjectTypeId = objectTo.ObjectTypeId,
                DisplayFrom = objectFrom.DisplayFrom,
                DisplayTo = objectTo.DisplayFrom
            };

            return result;
        }

        internal static ObjectModel ConvertToModel<T>(T outline, IDirectoryManager directoryManager)
        {
            if (typeof(T) == typeof(CropPlan.ListItem))
            {
                return ConvertToModelFromCropPlan(outline as CropPlan.ListItem, directoryManager);
            }
            else if (typeof(T) == typeof(OutlinesNetTcp))
            {
                return ConvertToModelFromOutline(outline as OutlinesNetTcp);
            }
            else if (typeof(T) == typeof(FieldVersion.ListItem))
            {
                return ConvertToModelFromField(outline as FieldVersion.ListItem);
            }
            return null;
        }

        private static ObjectModel ConvertToModelFromCropPlan(CropPlan.ListItem cropPlan, IDirectoryManager directoryManager)
        {
            var result = new ObjectModel
            {
                IsSingleObject = true,
                IsCropPlan = true,
                ObjectId = cropPlan.OutlineID
            };

            string fieldName = string.Empty;

            var fieldVersion = directoryManager.FieldVersions.FirstOrDefault(x => x.CropPlans.FirstOrDefault(y => y.Identity == cropPlan.Identity) != null);
            if (fieldVersion != null)
            {
                fieldName = fieldVersion.FVersField.FldNumber;
            }

            result.DisplayFrom = string.Format("Поле - {0}, Посівна площа - {1} ({2} га.)", fieldName, cropPlan.CropPlanCultCrop.CultCropName, cropPlan.CropPlanArea);

            return result;
        }

        private static ObjectModel ConvertToModelFromOutline(OutlinesNetTcp outline)
        {
            var result = new ObjectModel
            {
                IsSingleObject = true,
                IsCropPlan = false,
                ObjectId = outline.OutId,
                ObjectTypeId = outline.OutObjectTypeId,
                DisplayFrom = outline.OutNameWithType
            };

            return result;
        }

        private static ObjectModel ConvertToModelFromField(FieldVersion.ListItem outline)
        {
            var result = new ObjectModel
            {
                IsSingleObject = true,
                IsCropPlan = false,
                //_directoryManager.FieldVersionsInformation.FirstOrDefault(x=>x.FVersFieldId == outline.fi)
                ObjectId = outline.FVersOutlineId,
                ObjectTypeId = 8,
                DisplayFrom = string.Format("Поле - {0} ({1} га.)", outline.FVersField.FldNumber, outline.FVersArea)
            };

            return result;
        }

        public static ObjectModel ConvertToModelFromLaborDetailObject(LaborDetailObject.Item laborDetailObject, int Year, IDirectoryManager directoryManager)
        {
            var load = laborDetailObject.Loading.FirstOrDefault();
            ObjectModel objectFrom = ConvertToModelFromLaborDetailObjectItem(load, Year, directoryManager);
            if (objectFrom != null)
            {
                objectFrom.LaborDetailObjectID = laborDetailObject.Identity;
                objectFrom.LaborDetailObjectItemFromID = load.Identity;
            }

            var unload = laborDetailObject.Unloading.FirstOrDefault();
            ObjectModel objectTo = ConvertToModelFromLaborDetailObjectItem(unload, Year, directoryManager);
            if (objectTo != null)
            {
                objectTo.LaborDetailObjectID = laborDetailObject.Identity;
                objectTo.LaborDetailObjectItemToID = unload.Identity;
            }

            if (objectFrom != null && objectTo != null)
            {
                return ConvertToModel(objectFrom, objectTo);
            }

            return objectFrom;
        }

        private static ObjectModel ConvertToModelFromLaborDetailObjectItem(LaborDetailObjectItem.Item load, int Year, IDirectoryManager directoryManager)
        {
            ObjectModel objectFrom = null;

            if (load != null)
            {
                var outline = directoryManager.AllOutlines.FirstOrDefault(x => x.OutId == load.Outline.Identity);
                if (outline != null)
                {
                    if (outline.OutObjectTypeId == 8)
                    {
                        var field = directoryManager.FieldVersions.FirstOrDefault(x => x.FVersOutlineId == outline.OutId && (Year != 0 ? x.FVersYear == Year : true));
                        if (field != null)
                        {
                            objectFrom = ConvertToModelFromField(field);
                        }
                    }
                    else if (outline.OutObjectTypeId == 25)
                    {
                        var cropPlan = directoryManager.FieldVersions.Where(x => (Year != 0 ? x.FVersYear == Year : true)).SelectMany(x => x.CropPlans).FirstOrDefault(el => el.OutlineID == outline.OutId);
                        if (cropPlan != null)
                        {
                            objectFrom = ConvertToModelFromCropPlan(cropPlan, directoryManager);
                        }
                    }
                    else
                    {
                        objectFrom = ConvertToModelFromOutline(outline);
                    }
                }
            }

            return objectFrom;
        }

        #endregion

    }
}

