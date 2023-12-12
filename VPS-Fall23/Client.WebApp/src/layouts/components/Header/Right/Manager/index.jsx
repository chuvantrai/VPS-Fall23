import { Dropdown, Space } from 'antd';
import avatarImg from '@/assets/images/AvatarDefault.png';

const items = [
  {
    label: <a href="/change-password">Đổi mật khẩu</a>,
    key: '0',
  },
  {
    label: <a href="/login"><LogoutOutlined /> Đăng xuất</a>,
    key: '1',
  }
];

// eslint-disable-next-line react/prop-types
const ManagerRightHeader = ({ account }) => {
  return (
    <Dropdown menu={{ items }} trigger={['click']} placement="bottomRight">
      <span className={'h-[50px] cursor-pointer'} onClick={(e) => e.preventDefault()}>
        <Space>
          <img
            className={'w-[50px] h-[50px!important] rounded-[10px] object-cover'}
            src={account.Avatar === '' ? avatarImg : account.Avatar}
          />
        </Space>
      </span>
    </Dropdown>
  );
};

export default ManagerRightHeader;
