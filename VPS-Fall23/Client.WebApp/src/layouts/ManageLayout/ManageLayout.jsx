import { useState } from 'react';
import { Layout } from 'antd';

import Sidebar from '@/layouts/components/Sidebar';
import ContentLayout from '../components/Content/ContentLayout';

function ManageLayout() {
  const rowData = [
    {
      label: 'User',
      options: [
        { label: 'Profile', url: '/profile' },
        { label: 'test', url: '/' },
      ],
    },
    {
      label: 'Manage',
      options: [
        { label: 'View Parking Zone List', url: '/listParkingZone' },
        { label: 'Register Parking Zone', url: '/registerParkingZone' },
      ],
    },
  ];

  const [contentState, setContentState] = useState('1');

  return (
    <Layout>
      <Sidebar rowData={rowData} setContentState={setContentState} />
      <ContentLayout
        contentState={contentState}
        title={'Form Đăng ký bãi giữ xe'}
        desc={'Điền form dưới đây để đăng ký bãi giữ xe mới'}
      ></ContentLayout>
    </Layout>
  );
}

export default ManageLayout;
