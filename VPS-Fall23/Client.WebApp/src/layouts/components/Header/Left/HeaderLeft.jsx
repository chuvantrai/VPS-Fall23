import logo from '@/assets/logo/logo.png'
import { Link } from 'react-router-dom';
import styles from './HeaderLeft.module.scss';
import classNames from 'classnames/bind';
const cx = classNames.bind(styles);
const HeaderLeft = () => {
    return (<div className={cx('wrapper')}>
        <Link to="/">
            <img src={logo} />
        </Link>

    </div>)

}
export default HeaderLeft;