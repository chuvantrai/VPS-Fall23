import getAccountJwtModel from '@/helpers/getAccountJwtModel.js';
import DriverCenterHeader from './Driver';
import ManagerCenterHeader from './Manager';


const HeaderCenter = () => {
  const account = getAccountJwtModel();
  const headerCenterMap = {
    0: <DriverCenterHeader></DriverCenterHeader>,
    1: <ManagerCenterHeader></ManagerCenterHeader>,
    2: <ManagerCenterHeader></ManagerCenterHeader>
  }
  const getCenterHeader = () => {
    return headerCenterMap[account?.RoleId ?? 0]
  }
  return (
    <>
      {getCenterHeader()}
    </>
  );
};
export default HeaderCenter;
