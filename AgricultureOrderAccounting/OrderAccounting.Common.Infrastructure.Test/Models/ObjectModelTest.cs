using FMS.Services.NetTcpDirectoryServiceReference;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderAccounting.Common.Infrastructure.Models;

namespace OrderAccounting.Common.Infrastructure.Test.Models
{
    [TestClass]
    public class ObjectModelTest
    {
        [TestMethod]
        public void IsInputObjectValid_InvalidCropPlanData_False()
        {
            var cropPlanModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = true,
                ObjectId = 1
            };
            var fieldModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = false,
                ObjectId = 1,
                ObjectTypeId = 8
            };
            var outlineModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = false,
                ObjectId = 1,
                ObjectTypeId = 1
            };

            var inputModel = new CropPlansNetTcp()
            {
                CropPlanId = 1
            };

            //

            var cropPlanResult = cropPlanModel.IsInputObjectValid(inputModel);
            var fieldResult = fieldModel.IsInputObjectValid(inputModel);
            var outlineResult = outlineModel.IsInputObjectValid(inputModel);

            //

            Assert.IsFalse(cropPlanResult);
            Assert.IsTrue(fieldResult);
            Assert.IsTrue(outlineResult);
        }

        [TestMethod]
        public void IsInputObjectValid_ValidCropPlanData_True()
        {
            var cropPlanModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = true,
                ObjectId = 1
            };
            var fieldModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = false,
                ObjectId = 1,
                ObjectTypeId = 8
            };
            var outlineModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = false,
                ObjectId = 1,
                ObjectTypeId = 1
            };

            var inputModel = new CropPlansNetTcp()
            {
                CropPlanId = 2
            };

            //

            var cropPlanResult = cropPlanModel.IsInputObjectValid(inputModel);
            var fieldResult = fieldModel.IsInputObjectValid(inputModel);
            var outlineResult = outlineModel.IsInputObjectValid(inputModel);

            //

            Assert.IsTrue(cropPlanResult);
            Assert.IsTrue(fieldResult);
            Assert.IsTrue(outlineResult);
        }

        [TestMethod]
        public void IsInputObjectValid_InvalidFieldData_False()
        {
            var cropPlanModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = true,
                ObjectId = 1
            };
            var fieldModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = false,
                ObjectId = 1,
                ObjectTypeId = 8
            };
            var outlineModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = false,
                ObjectId = 1,
                ObjectTypeId = 1
            };

            var inputModel = new FieldsNetTcp()
            {
                FldId = 1
            };

            //

            var cropPlanResult = cropPlanModel.IsInputObjectValid(inputModel);
            var fieldResult = fieldModel.IsInputObjectValid(inputModel);
            var outlineResult = outlineModel.IsInputObjectValid(inputModel);

            //

            Assert.IsTrue(cropPlanResult);
            Assert.IsFalse(fieldResult);
            Assert.IsTrue(outlineResult);
        }

        [TestMethod]
        public void IsInputObjectValid_ValidFieldData_True()
        {
            var cropPlanModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = true,
                ObjectId = 1
            };
            var fieldModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = false,
                ObjectId = 1,
                ObjectTypeId = 8
            };
            var outlineModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = false,
                ObjectId = 1,
                ObjectTypeId = 1
            };

            var inputModel = new FieldsNetTcp()
            {
                FldId = 2
            };

            //

            var cropPlanResult = cropPlanModel.IsInputObjectValid(inputModel);
            var fieldResult = fieldModel.IsInputObjectValid(inputModel);
            var outlineResult = outlineModel.IsInputObjectValid(inputModel);

            //

            Assert.IsTrue(cropPlanResult);
            Assert.IsTrue(fieldResult);
            Assert.IsTrue(outlineResult);
        }

        [TestMethod]
        public void IsInputObjectValid_ValidOutlineTypeData_True()
        {
            var cropPlanModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = true,
                ObjectId = 1
            };
            var fieldModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = false,
                ObjectId = 1,
                ObjectTypeId = 8
            };
            var outlineModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = false,
                ObjectId = 1,
                ObjectTypeId = 1
            };

            var inputModel = new OutlinesNetTcp()
            {
                OutId = 1,
                OutObjectTypeId = 2
            };

            //

            var cropPlanResult = cropPlanModel.IsInputObjectValid(inputModel);
            var fieldResult = fieldModel.IsInputObjectValid(inputModel);
            var outlineResult = outlineModel.IsInputObjectValid(inputModel);

            //

            Assert.IsTrue(cropPlanResult);
            Assert.IsTrue(fieldResult);
            Assert.IsTrue(outlineResult);
        }

        [TestMethod]
        public void IsInputObjectValid_InvalidOutlineData_False()
        {
            var cropPlanModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = true,
                ObjectId = 1
            };
            var fieldModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = false,
                ObjectId = 1,
                ObjectTypeId = 8
            };
            var outlineModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = false,
                ObjectId = 1,
                ObjectTypeId = 1
            };

            var inputModel = new OutlinesNetTcp()
            {
                OutId = 1,
                OutObjectTypeId = 1
            };

            //

            var cropPlanResult = cropPlanModel.IsInputObjectValid(inputModel);
            var fieldResult = fieldModel.IsInputObjectValid(inputModel);
            var outlineResult = outlineModel.IsInputObjectValid(inputModel);

            //

            Assert.IsTrue(cropPlanResult);
            Assert.IsTrue(fieldResult);
            Assert.IsFalse(outlineResult);
        }

        [TestMethod]
        public void IsInputObjectValid_ValidOutlineData_True()
        {
            var cropPlanModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = true,
                ObjectId = 1
            };
            var fieldModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = false,
                ObjectId = 1,
                ObjectTypeId = 8
            };
            var outlineModel = new ObjectModel()
            {
                IsSingleObject = true,
                IsCropPlan = false,
                ObjectId = 1,
                ObjectTypeId = 1
            };

            var inputModel = new OutlinesNetTcp()
            {
                OutId = 2,
                OutObjectTypeId = 1
            };

            //

            var cropPlanResult = cropPlanModel.IsInputObjectValid(inputModel);
            var fieldResult = fieldModel.IsInputObjectValid(inputModel);
            var outlineResult = outlineModel.IsInputObjectValid(inputModel);

            //

            Assert.IsTrue(cropPlanResult);
            Assert.IsTrue(fieldResult);
            Assert.IsTrue(outlineResult);
        }

        [TestMethod]
        public void IsInputDoubleObjectValid_ValidTypeObject_True()
        {
            var objectModel = new ObjectModel()
            {
                IsSingleObject = false,
                ObjectId = 1,
                ObjectTypeId = 1,
                ToObjectId = 2,
                ToObjectTypeId = 2
            };

            var inputFromModel = new ObjectModel()
            {
                ObjectId = 11,
                ObjectTypeId = 1
            };
            var inputToModel = new ObjectModel()
            {
                ObjectId = 21,
                ObjectTypeId = 2
            };

            //

            var result = objectModel.IsInputDoubleObjectValid(inputFromModel, inputToModel);

            //

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsInputDoubleObjectValid_ValidToObject_True()
        {
            var objectModel = new ObjectModel()
            {
                IsSingleObject = false,
                ObjectId = 1,
                ObjectTypeId = 1,
                ToObjectId = 2,
                ToObjectTypeId = 2
            };

            var inputFromModel = new ObjectModel()
            {
                ObjectId = 1,
                ObjectTypeId = 1
            };
            var inputToModel = new ObjectModel()
            {
                ObjectId = 2,
                ObjectTypeId = 22
            };

            //

            var result = objectModel.IsInputDoubleObjectValid(inputFromModel, inputToModel);

            //

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsInputDoubleObjectValid_ValidFromObject_True()
        {
            var objectModel = new ObjectModel()
            {
                IsSingleObject = false,
                ObjectId = 1,
                ObjectTypeId = 1,
                ToObjectId = 2,
                ToObjectTypeId = 2
            };

            var inputFromModel = new ObjectModel()
            {
                ObjectId = 1,
                ObjectTypeId = 11
            };
            var inputToModel = new ObjectModel()
            {
                ObjectId = 2,
                ObjectTypeId = 2
            };

            //

            var result = objectModel.IsInputDoubleObjectValid(inputFromModel, inputToModel);

            //

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsInputDoubleObjectValid_InvalidObjects_False()
        {
            var objectModel = new ObjectModel()
            {
                IsSingleObject = false,
                ObjectId = 1,
                ObjectTypeId = 1,
                ToObjectId = 2,
                ToObjectTypeId = 2
            };

            var inputFromModel = new ObjectModel()
            {
                ObjectId = 1,
                ObjectTypeId = 1
            };
            var inputToModel = new ObjectModel()
            {
                ObjectId = 2,
                ObjectTypeId = 2
            };

            //

            var result = objectModel.IsInputDoubleObjectValid(inputFromModel, inputToModel);

            //

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Equals_EqualObjects_True()
        {
            var objectModel = new ObjectModel()
            {
                ObjectId = 1,
                ObjectTypeId = 1
            };
            var inputModel = new ObjectModel()
            {
                ObjectId = 1,
                ObjectTypeId = 1
            };

            ///

            var result = objectModel.Equals(inputModel);

            ///

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_EqualObjectsWithoutType_True()
        {
            var objectModel = new ObjectModel()
            {
                ObjectId = 1
            };
            var inputModel = new ObjectModel()
            {
                ObjectId = 1
            };

            ///

            var result = objectModel.Equals(inputModel);

            ///

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_UnEqualObjectsWithSameTypes_False()
        {
            var objectModel = new ObjectModel()
            {
                ObjectId = 1,
                ObjectTypeId = 1
            };
            var inputModel = new ObjectModel()
            {
                ObjectId = 5,
                ObjectTypeId = 1
            };

            ///

            var result = objectModel.Equals(inputModel);

            ///

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Equals_UnEqualObjectsWithoutTypes_False()
        {
            var objectModel = new ObjectModel()
            {
                ObjectId = 1
            };
            var inputModel = new ObjectModel()
            {
                ObjectId = 5
            };

            ///

            var result = objectModel.Equals(inputModel);

            ///

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Equals_UnEqualObjectsWithDifferentTypes_False()
        {
            var objectModel = new ObjectModel()
            {
                ObjectId = 1,
                ObjectTypeId = 1
            };
            var inputModel = new ObjectModel()
            {
                ObjectId = 1,
                ObjectTypeId = 2
            };

            ///

            var result = objectModel.Equals(inputModel);

            ///

            Assert.IsFalse(result);
        }

    }
}
