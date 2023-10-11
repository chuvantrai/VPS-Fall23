import styles from './Footer.module.scss'
import classNames from 'classnames/bind';
import { Footer as AntdFooter } from 'antd/es/layout/layout';

const cx = classNames.bind(styles);
const Footer = () => {
    return (<AntdFooter style={{ textAlign: 'center' }}>Ant Design ©2023 Created by Ant UED</AntdFooter>)
}
export default Footer