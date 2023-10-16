import classNames from 'classnames/bind';
import PropTypes from 'prop-types';
import { Layout } from 'antd';

import styles from './DefaultLayout.module.scss';
import Header from '@/layouts/components/Header';
import ManageLayout from '../ManageLayout/ManageLayout';

const cx = classNames.bind(styles);

function DefaultLayout() {
  return (
    <Layout className={cx('wrapper w-full min-h-screen')}>
      <Header />
      <ManageLayout></ManageLayout>
    </Layout>
  );
}

DefaultLayout.propTypes = {
  children: PropTypes.node.isRequired,
};

export default DefaultLayout;
