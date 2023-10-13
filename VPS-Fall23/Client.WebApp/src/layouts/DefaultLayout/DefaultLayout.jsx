import { useState } from 'react';
import classNames from 'classnames/bind';
import PropTypes from 'prop-types';
import { Layout } from 'antd';

import styles from './DefaultLayout.module.scss';
import Header from '@/layouts/components/Header';
import Sidebar from '@/layouts/components/Sidebar';
import ContentLayout from '../components/Content/ContentLayout';

const cx = classNames.bind(styles);

function DefaultLayout() {
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
    <Layout className={cx('wrapper w-full min-h-screen')}>
      <Header />
      <Layout>
        <Sidebar rowData={rowData} setContentState={setContentState} />
        <ContentLayout contentState={contentState}></ContentLayout>
      </Layout>
    </Layout>
  );
}

DefaultLayout.propTypes = {
  children: PropTypes.node.isRequired,
};

export default DefaultLayout;
