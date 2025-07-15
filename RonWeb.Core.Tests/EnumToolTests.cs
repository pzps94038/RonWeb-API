using System.ComponentModel;
using RonWeb.Core;
using Xunit;

namespace RonWeb.Core.Tests
{
    public class EnumToolTests
    {
        public enum TestEnum
        {
            [Description("First Value")]
            FirstValue,
            
            [Description("Second Value")]
            SecondValue,
            
            // This one has no Description attribute
            ThirdValue
        }

        public enum EmptyEnum
        {
            NoDescription
        }

        [Fact]
        public void Description_WithDescriptionAttribute_ShouldReturnDescription()
        {
            // Act
            var description = TestEnum.FirstValue.Description();

            // Assert
            Assert.Equal("First Value", description);
        }

        [Fact]
        public void Description_WithoutDescriptionAttribute_ShouldReturnEnumName()
        {
            // Act
            var description = TestEnum.ThirdValue.Description();

            // Assert
            Assert.Equal("ThirdValue", description);
        }

        [Fact]
        public void Description_WithDifferentEnumValues_ShouldReturnCorrectDescriptions()
        {
            // Act & Assert
            Assert.Equal("First Value", TestEnum.FirstValue.Description());
            Assert.Equal("Second Value", TestEnum.SecondValue.Description());
            Assert.Equal("ThirdValue", TestEnum.ThirdValue.Description());
        }

        [Fact]
        public void Description_WithEmptyEnum_ShouldReturnEnumName()
        {
            // Act
            var description = EmptyEnum.NoDescription.Description();

            // Assert
            Assert.Equal("NoDescription", description);
        }

        [Fact]
        public void Description_WithEmptyDescriptionAttribute_ShouldReturnEmptyString()
        {
            // This tests the scenario where Description attribute exists but has empty description
            // We'll create a test enum for this specific case
            var enumValue = TestEnumWithEmptyDescription.EmptyDesc;
            
            // Act
            var description = enumValue.Description();

            // Assert
            Assert.Equal("", description);
        }

        public enum TestEnumWithEmptyDescription
        {
            [Description("")]
            EmptyDesc
        }
    }
}