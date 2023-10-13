import { useState } from 'react';
import { Layout } from 'antd';

import Sidebar from '@/layouts/components/Sidebar';
import ContentLayout from '../components/Content/ContentLayout';

function ManageLayout() {
  const rowData = [
    {
      label: 'User',
      options: ['Profile', 'Test'],
    },
    {
      label: 'Manage',
      options: ['View Parking Zone List'],
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
