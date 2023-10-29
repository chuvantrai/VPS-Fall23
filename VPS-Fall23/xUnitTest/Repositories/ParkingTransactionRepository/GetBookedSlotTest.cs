using Xunit.Abstractions;

namespace xUnitTest.Repositories.ParkingTransactionRepository
{
    public class GetBookedSlotTest : Base
    {
        public GetBookedSlotTest(ITestOutputHelper output) : base(output)
        {
        }
        [Fact]
        public async Task BookedSlotRightNumber()
        {
            // Arrange
            var parkingZoneId = parkingZoneIdHaveBookedSlot;
            var checkAt = TimeHaveBookedSlot;
            int expectedCount = 10;
            // Act
            var result = await repository.GetBookedSlot(parkingZoneId, checkAt);

            // Assert
            Assert.Equal(expectedCount, result);
        }
        [Fact]
        public async Task NotExistingParkingZone()
        {
            // Arrange
            var parkingZoneId = Guid.Empty;
            var checkAt = DateTime.Now;

            // Act
            var result = await repository.GetBookedSlot(parkingZoneId, checkAt);

            // Assert
            Assert.Equal(0, result);
        }
        [Fact]
        public async Task ParkingZoneNotHaveBookedSlot()
        {
            var parkingZoneId = parkingZoneIdHaveBookedSlot;
            var checkAt = TimeDoNotHaveBookedSlot;
            var result = await repository.GetBookedSlot(parkingZoneId, checkAt);

            Assert.Equal(0, result);
        }
    }
}
