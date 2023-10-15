import { useState } from 'react';
import { Layout } from 'antd';

import Sidebar from '@/layouts/components/Sidebar';
import ContentLayout from '@/layouts/components/Content/ContentLayout';
import config from '@/config';

function ManageLayout() {
  let rowData = [
    {
      label: '',
      options: [
        {
          label: '',
          url: '',
          title: '',
          desc: '',
        },
      ],
    },
  ];
  rowData = config.adminSidebar;

  const [selectedURL, setSelectedURL] = useState({ label: '', url: '' });
  const [contentItem, setcontentItem] = useState({
    label: '',
    url: '',
    title: '',
    desc: '',
  });

  const onChangeURL = (e) => {
    setSelectedURL(e.url);
    let label = e.label;
    let item = [
      {
        label: '',
        options: [
          {
            label: '',
            url: '',
            title: '',
            desc: '',
          },
        ],
      },
    ];
    item = rowData.find((item) => item.label == label);
    setcontentItem(item.options.find((i) => i.url == e.url));
  };

  return (
    <Layout>
      <Sidebar rowData={rowData} setSelectedKey={onChangeURL} />
      {selectedURL !== undefined && <ContentLayout title={contentItem.title} desc={contentItem.desc}></ContentLayout>}
    </Layout>
  );
}

export default ManageLayout;
