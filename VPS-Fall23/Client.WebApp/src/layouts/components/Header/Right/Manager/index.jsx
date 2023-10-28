import { Dropdown, Space } from 'antd';
const items = [
  {
    label: <a href="/login">Đăng xuất</a>,
    key: '0',
  },
];

const ManagerRightHeader = ({ account }) => {
  return (
    <Dropdown menu={{ items }} trigger={['click']} placement="bottomRight">
      <span className={'h-[50px] cursor-pointer'} onClick={(e) => e.preventDefault()}>
        <Space>
          <img
            className={'w-[50px] h-[50px!important] rounded-[10px] object-cover'}
            src={account.Avatar === '' ? '../src/assets/images/AvatarDefault.png' : account.Avatar}
          />
        </Space>
      </span>
    </Dropdown>
  );
};

export default ManagerRightHeader;
