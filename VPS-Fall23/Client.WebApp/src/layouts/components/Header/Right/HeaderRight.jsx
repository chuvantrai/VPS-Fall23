import classNames from 'classnames/bind';
import styles from './HeaderRight.module.scss';
import getAccountJwtModel from '@/helpers/getAccountJwtModel.js';
import DriverRightHeader from './Driver';
import ManagerRightHeader from './Manager';

const cs = classNames.bind(styles);

const HeaderRight = () => {
  const account = getAccountJwtModel();

  return (
    <div className={cs('wrapper flex justify-end h-full items-center')}>
      {
        account ?
          <ManagerRightHeader account={account}></ManagerRightHeader>
          :
          <DriverRightHeader></DriverRightHeader>
      }
    </div>
  );
};
export default HeaderRight;
