import classNames from "classnames/bind"
import styles from './HeaderRight.module.scss';
import { Button, Typography } from "antd";
const cs = classNames.bind(styles)
const { Text } = Typography;
const HeaderRight = () => {
    return (<div className={cs('wrapper')}>
        <Text>Bạn muốn quản lý nhà xe của bạn? <Button>Đăng ký</Button>
        </Text>
    </div>)
}
export default HeaderRight