import classNames from 'classnames/bind';
import styles from './HeaderRight.module.scss';
import getAccountJwtModel from '@/helpers/getAccountJwtModel.js';
import DriverRightHeader from './Driver';
import ManagerRightHeader from './Manager';

const cs = classNames.bind(styles);

const HeaderRight = () => {
  const account = getAccountJwtModel();
  const headerRightMap = {
    0: <DriverRightHeader />,
    1: <ManagerRightHeader account={account} />,
    2: <ManagerRightHeader account={account} />
  }

  const getHeaderRightMap = () => {
    return headerRightMap[account?.RoleId ?? 0]
  }
  return (
    <div className={cs('wrapper flex justify-end h-full items-center')}>
      {getHeaderRightMap()}
    </div>
  );
};
export default HeaderRight;
