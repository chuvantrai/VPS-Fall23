import classNames from 'classnames/bind';
import PropTypes from 'prop-types';
import { Layout } from 'antd';
import Footer from '@/layouts/components/Footer';
import styles from './DefaultLayout.module.scss';
import Header from '@/layouts/components/Header';
import ManageLayout from '../ManageLayout/ManageLayout';
import getAccountJwtModel from '@/helpers/getAccountJwtModel.js';
import DriverHompage from '@/pages/Homepage/components/DriverHompage';
import Sidebar from '@/layouts/components/Sidebar';
import config from '@/config';
import { useState } from 'react';
const cx = classNames.bind(styles);
const getSideBar = (account, setSelectedURLCallback, setContentItemCallback) => {
  if (!account) {
    return (<></>)
  }
  const onChangeURL = (e) => {
    setSelectedURLCallback(e.url);
    const itemFoundWithLabel = config.adminSidebar.find((item) => item.label == e.label)
    const contentItemWithUrl = itemFoundWithLabel.options.find((i) => i.url == e.url);
    setContentItemCallback(contentItemWithUrl);
  };
  return (<Sidebar rowData={config.adminSidebar} setSelectedKey={onChangeURL} />)
}

const defaultContentItem = {
  label: '',
  url: '',
  title: '',
  desc: '',
}

const defaultSelectedUrl = {
  label: '',
  url: ''
}

function DefaultLayout() {
  const account = getAccountJwtModel();
  const [selectedURL, setSelectedURL] = useState(defaultSelectedUrl);
  const [contentItem, setContentItem] = useState(defaultContentItem);

  return (
    <Layout className={cx('wrapper w-full min-h-screen')}>
      <Header />
      <Layout style={{ position: 'relative' }}>
        {getSideBar(account, setSelectedURL, setContentItem)}
        {account
          ?
          <ManageLayout contentItem={contentItem} isShow={selectedURL} />
          :
          <DriverHompage />
        }
      </Layout>
      {account ? <></> : <Footer />}
    </Layout>
  );
}

DefaultLayout.propTypes = {
  children: PropTypes.node.isRequired,
};

export default DefaultLayout;
