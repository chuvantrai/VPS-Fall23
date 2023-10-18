import getAccountJwtModel from '@/helpers/getAccountJwtModel.js';
import DriverCenterHeader from './Driver';
import ManagerCenterHeader from './Manager';
const HeaderCenter = () => {
  const account = getAccountJwtModel();

  return (
    <>
      {
        account
          ?
          <ManagerCenterHeader></ManagerCenterHeader>
          :
          <DriverCenterHeader></DriverCenterHeader>
      }
    </>
  );
};
export default HeaderCenter;
