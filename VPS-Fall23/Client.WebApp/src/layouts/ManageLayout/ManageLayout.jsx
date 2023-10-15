import { useState } from 'react';
import { Layout } from 'antd';

import Sidebar from '@/layouts/components/Sidebar';
import ContentLayout from '@/layouts/components/Content/ContentLayout';
import config from '@/config';

function ManageLayout() {
  const rowData = config.adminSidebar;

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
