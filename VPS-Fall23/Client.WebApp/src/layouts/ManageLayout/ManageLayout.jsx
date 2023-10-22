import { Layout } from 'antd';
import PropTypes from 'prop-types';

import ContentHeader from '@/layouts/components/ContentHeader/ContentHeader';
import ContentLayout from '@/layouts/components/Content/ContentLayout';

function ManageLayout({ isShow, contentItem }) {
  return (
    <Layout>
      <ContentHeader title={contentItem.title} desc={contentItem.desc} />
      <div className="h-fit bg-[#f0f2f5] px-[24px] mb-[20px]">
        {isShow && <ContentLayout></ContentLayout>}
      </div>
    </Layout>
  );
}

ManageLayout.propTypes = {
  contentItem: PropTypes.object
}

export default ManageLayout;
