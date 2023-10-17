import classNames from 'classnames/bind';
import { Button, Dropdown, Space, Typography } from 'antd';
import styles from './HeaderRight.module.scss';
import getAccountJwtModel from '@/helpers/getAccountJwtModel.js';

const cs = classNames.bind(styles);
const { Text } = Typography;
const items = [
  {
    label: <a href='/login'>Đăng xuất</a>,
    key: '0',
  },
];
const HeaderRight = () => {
  const account = getAccountJwtModel();

  return (
    <div className={cs('wrapper flex justify-end h-full items-center')}>
      {account == null ?
        <Text>
          Bạn muốn quản lý nhà xe của bạn?
          <a href='/login'>
            <Button
              type='primary'
              htmlType='button'
              style={{
                backgroundColor: '#1677ff',
                marginLeft: '10px',
              }}
            >
              Đăng Nhập
            </Button>
          </a>
        </Text> :
        <Dropdown
          menu={{ items }}
          trigger={['click']}
          placement='bottomRight'
        >
          <span className={'h-[50px] cursor-pointer'} onClick={(e) => e.preventDefault()}>
            <Space>
               <img
                 className={'w-[50px] h-[50px!important] rounded-[50%] object-cover'}
                 src={account.Avatar ?? '../src/assets/images/AvatarDefault.png'} />
            </Space>
          </span>
        </Dropdown>
      }
    </div>
  );
};
export default HeaderRight;
