import { Fragment, useState, useEffect } from 'react';
import IncomeDashboard from '../IncomeDashboard/IncomeDashboard';
import BookedOverview from './BookedOverview';
import { Select } from 'antd';
import { getAccountJwtModel } from '@/helpers';
import useParkingTransactionService from '@/services/parkingTransactionSerivce';
import useParkingZoneService from '@/services/parkingZoneService';

function Overview() {
  const parkingZoneService = useParkingZoneService();
  const account = getAccountJwtModel();
  const parkingTransactionService = useParkingTransactionService();

  const { Option } = Select;
  const [selectedParkingZone, setSelectedParkingZone] = useState('');
  const [parkingZoneId, setParkingZoneId] = useState('');
  const [ParkingZoneOptions, setParkingZoneOptions] = useState([]);
  const [ParkingZoneData, setParkingZoneData] = useState([]);

  useEffect(() => {
    parkingZoneService
      .getAllParkingZoneByOwnerId(account.UserId)
      .then((response) => {
        setParkingZoneOptions(response.data);
        console.log(response.data);
      })
      .catch((error) => {
        console.error('Error fetching parking zones:', error);
      });
  }, []);

  const handleSelectChange = (value) => {
    setSelectedParkingZone(value);
    setParkingZoneId(value);

    parkingTransactionService
      .getAllIncome(selectedParkingZone, account.UserId)
      .then((response) => {
        setParkingZoneData(response.data);
      })
      .catch((error) => {
        console.error('Error fetching data:', error);
      });
  };

  return (
    <Fragment>
      <div>
        <Select className="mb-5" value={selectedParkingZone} onChange={handleSelectChange} style={{ width: '200px' }}>
          <Option value="">Tất cả bãi đỗ xe</Option>
          {ParkingZoneOptions.map((option) => (
            <Option key={option.value} value={option.value}>
              {option.label}
            </Option>
          ))}
        </Select>
        <BookedOverview parkingZoneId={parkingZoneId}></BookedOverview>
        <IncomeDashboard selectedParkingZone={selectedParkingZone} ParkingZoneData={ParkingZoneData}></IncomeDashboard>
      </div>
    </Fragment>
  );
}

export default Overview;
