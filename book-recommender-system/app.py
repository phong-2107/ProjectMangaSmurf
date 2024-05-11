from flask import Flask,render_template,request, jsonify
import pickle
import numpy as np
# from flask_cors import CORS

app = Flask(__name__)

# load dữ liệu
popular_df = pickle.load(open('popular.pkl','rb'))
pt = pickle.load(open('pt.pkl','rb'))
books = pickle.load(open('books.pkl','rb'))
similarity_scores = pickle.load(open('similarity_scores.pkl','rb'))

@app.route('/api/recommend_books', methods=['POST'])
def recommends():
    user_input = request.form.get('user_input')
    indices = np.where(pt.index == user_input)[0]

    if indices.size > 0:
        index = indices[0]
        similar_items = sorted(list(enumerate(similarity_scores[index])), key=lambda x: x[1], reverse=True)[1:5]

        data = []
        for i in similar_items:
            book_id = pt.index[i[0]]
            book_data = books[books['Book-Id'] == book_id].iloc[0]
            data.append({
                'title': book_data['Book-Title'],
                'author': book_data['Book-Author'],
                'image_url': book_data.get('Image-URL-M', 'default.jpg'),
                'votes': book_data['num_ratings'],
                'rating': book_data['avg_rating']
            })
    else:
        data = {'error': 'No matching entries found'}

    return jsonify(data)


@app.route('/')
def index():
    return render_template('index.html',
                           book_name = list(popular_df['Book-Id'].values),
                           author=list(popular_df['Book-Author'].values),
                           votes=list(popular_df['num_ratings'].values),
                           rating=list(popular_df['avg_rating'].values)
                           )

@app.route('/recommend')
def recommend_ui():
    return render_template('recommend.html')

@app.route('/recommend_books',methods=['post'])
def recommend():
    user_input = request.form.get('user_input')
    index = np.where(pt.index == user_input)[0][0]
    similar_items = sorted(list(enumerate(similarity_scores[index])), key=lambda x: x[1], reverse=True)[1:5]

    data = []
    for i in similar_items:
        item = []
        temp_df = books[books['Book-Title'] == pt.index[i[0]]]
        item.extend(list(temp_df.drop_duplicates('Book-Title')['Book-Title'].values))
        item.extend(list(temp_df.drop_duplicates('Book-Title')['Book-Author'].values))
        item.extend(list(temp_df.drop_duplicates('Book-Title')['Image-URL-M'].values))

        data.append(item)

    print(data)

    return render_template('recommend.html',data=data)

# API goi y sach
@app.route('/goi-y-sach', methods=['POST'])
def goi_y_sach():
    # Kiểm tra xem request có phải là POST không
    if request.method == 'POST':

        if request.is_json:
            data = request.get_json()
            # print(data)
            # Kiểm tra xem key 'book-id' có trong dữ liệu không
            if 'Book-Id' in data:
                book_id = data['Book-Id']
                try:
                    recommended_books = recommend(book_id)
                    if recommended_books is not None:
                        # Chuyển đổi recommended_books thành một đối tượng có thể chuyển đổi thành JSON
                        # Chuyển đổi các giá trị số thành mảng int
                        recommended_books_json = [int(item[0]) for item in recommended_books]
                        # recommended_books_json = json.dumps(recommended_books_json, cls=NpEncoder)

                        response = {
                            "status": 200,
                            "recommended_books_id": recommended_books_json
                        }
                        return jsonify(response), 200
                    else:
                        response = {
                            "status": 400,
                            "message": "Book ID not found or error occurred"
                        }
                        return jsonify(response), 400
                except Exception as e:
                    # Bắt ngoại lệ và in ra thông tin chi tiết về lỗi
                    print("An error occurred:", str(e))
                    response = {
                        "status": 400,
                        "message": "An error occurred"
                    }
                    return jsonify(response), 400
            else:
                response = {
                    "status": 400,
                    "message": "'Book-Id' key not found in request"
                }
                return jsonify(response), 400
        else:
            response = {
                "status": 400,
                "message": "Request must be in JSON format"
            }
            return jsonify(response), 400
    else:
        response = {
            "status": 405,
            "message": "Method Not Allowed"
        }
        return jsonify(response), 405

def recommend(book_id):
    book_id = int(book_id)
    # Kiểm tra xem book_name có tồn tại trong pt.index không
    if book_id in pt.index:
        # index fetch
        index = np.where(pt.index == book_id)[0][0]
        similar_items = sorted(list(enumerate(similarity_scores[index])), key=lambda x: x[1], reverse=True)[1:6]
        
        data = []
        for i in similar_items:
            item = []
            temp_df = books[books['Book-Id'] == pt.index[i[0]]]
            item.extend(list(temp_df.drop_duplicates('Book-Title')['Book-Id'].values))
            
            data.append(item)
        
        return data
    else:
        print("Book not found in database")
        return None


if __name__ == '__main__':
    app.run(debug=True)