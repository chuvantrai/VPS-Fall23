import PropTypes from 'prop-types';
import Footer from "@/layouts/components/Footer";
import Header from '@/layouts/components/Header';
import Layout, { Content } from 'antd/es/layout/layout';

function HeaderOnly({ children }) {
  return (

    <Layout style={{ minHeight: "100vh" }}>
      <Header />
      <Content style={{ position: "relative" }}>
        {children}
      </Content>
      <Footer></Footer>
    </Layout>

  );
}

HeaderOnly.propTypes = {
  children: PropTypes.node.isRequired,
};

export default HeaderOnly;
